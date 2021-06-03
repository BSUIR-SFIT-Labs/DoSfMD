package com.shulga.androidapp.services

import android.net.Uri
import com.google.firebase.auth.FirebaseAuth
import com.google.firebase.auth.ktx.auth
import com.google.firebase.ktx.Firebase
import com.shulga.androidapp.models.ComputerAsset
import com.shulga.androidapp.models.ComputersDashboard
import com.shulga.androidapp.models.auth.Auth
import com.shulga.androidapp.models.auth.User
import com.shulga.androidapp.models.auth.UsersDashboard
import java.lang.Exception

object SessionService {
    private var songsAuth: Auth? = null
    private var dashboard: ComputersDashboard? = null

    private var initialized: Boolean = false

    private var songsUser: User? = null
    private var usersDashboard: UsersDashboard? = UsersDashboard()

    private val songsUserService = UserFirebaseService()
    private val songsAssetFirebaseService = ComputerAssetFirebaseService()

    var selectedAsset: ComputerAsset? = null
    var selectedUser: User? = null

    fun isInitialized(): Boolean = initialized

    fun getLocalAssets(): ArrayList<ComputerAsset>? {
        return dashboard?.assets
    }

    fun getLocalAsset(id: String): ComputerAsset? {
        return getLocalAssets()?.first { asset ->
            asset.id == id
        }
    }

    private fun deleteLocalAsset(asset: ComputerAsset) {
        val index = getLocalAssets()?.indexOfFirst { asset0 ->
            asset0.id == asset.id
        }
        if (index != null && index != -1) {
            dashboard?.assets?.removeAt(index)
            if (selectedAsset?.id == asset.id) {
                selectedAsset = null
            }
        }
    }

    fun deleteRemoteAsset(asset: ComputerAsset, completion: (Exception?) -> Unit) {
        songsAssetFirebaseService.deleteRemoteAsset(asset) { error ->
            if (error != null) {
                println(error)
                completion(error)
            } else {
                this.deleteLocalAsset(asset)
                completion(null)
            }
        }
    }

    private fun updateLocalAsset(asset: ComputerAsset) {
        val index = getLocalAssets()?.indexOfFirst { asset0 ->
            asset0.id == asset.id
        }
        if (index != null && index != -1) {
            dashboard?.assets?.set(index, asset)
            if (selectedAsset?.id == asset.id) {
                selectedAsset = asset
            }
        } else {
            dashboard?.assets?.add(asset)
        }
    }

    fun updateRemoteAsset(asset: ComputerAsset, photoUri: Uri?, videoUri: Uri?, completion: (Exception?) -> Unit) {
        songsAssetFirebaseService.updateRemoteAsset(asset, photoUri, videoUri) { updatedAsset, error ->
            if (error != null) {
                println(error)
                completion(error)
            } else if (updatedAsset != null) {
                this.updateLocalAsset(updatedAsset)
                completion(null)
            } else {
                completion(Exception("Invalid updateRemoteAsset form SongsAssetFirebaseService closure return"))
            }
        }
    }

    fun syncDashboard(onCompleted: () -> Unit) {
        songsAssetFirebaseService.getRemoteAssets { assets, error ->
            if (error != null) {
                println(error)
                this.dashboard?.assets = ArrayList()
                this.selectedAsset = null
            } else if (assets != null) {
                this.dashboard?.assets = assets
                if (selectedAsset != null) {
                    this.selectedAsset = getLocalAsset(selectedAsset!!.id)
                }
            } else {
                println("Didn't receive assets and error")
                this.dashboard?.assets = ArrayList()
                this.selectedAsset = null
            }
            onCompleted()
        }
    }

    private fun initialize(songsAuth: Auth, onCompleted: () -> Unit) {
        this.songsAuth = songsAuth

        AuthStorage.save(songsAuth)

        dashboard = ComputersDashboard()

        syncDashboard {
            this.initialized = true
            onCompleted()
        }

    }

    fun destroy() {
        initialized = false

        try {
            FirebaseAuth.getInstance().signOut()
        } catch (error: Throwable) {
            println(error)
        }

        AuthStorage.delete()
        selectedAsset = null
        selectedUser = null
        songsAuth = null
        dashboard = null
        songsUser = null
        usersDashboard = UsersDashboard()
    }

    fun restore(completion: (Exception?) -> Unit): Auth? {
        val songsAuth = AuthStorage.restore()
        if (songsAuth != null) {
            signInEmail(songsAuth.email, songsAuth.password) { error ->
                this.handleFirebaseAuthResponse(songsAuth, error, completion)
            }
            return songsAuth
        } else {
            completion(Exception("Unable to restore session"))
            return null
        }
    }

    fun signUpEmail(email: String, password: String, completion: (Exception?) -> Unit) {
        val songsAuth = Auth(email, password)
        FirebaseAuth.getInstance()
            .createUserWithEmailAndPassword(email, password)
            .addOnSuccessListener { _ ->
                this.handleFirebaseAuthResponse(songsAuth, null, completion)
                this.updateRemoteUserAuth(songsAuth, null, completion)
            }
            .addOnFailureListener { e ->
                this.handleFirebaseAuthResponse(songsAuth, e, completion)
                this.updateRemoteUserAuth(songsAuth, e, completion)
            }
    }

    fun signInEmail(email: String, password: String, completion: (Exception?) -> Unit) {
        val songsAuth = Auth(email, password)
        FirebaseAuth.getInstance()
            .signInWithEmailAndPassword(email, password)
            .addOnSuccessListener { _ ->
                this.handleFirebaseAuthResponse(songsAuth, null, completion)
            }
            .addOnFailureListener { e ->
                this.handleFirebaseAuthResponse(songsAuth, e, completion)
            }
    }

    fun getSongsUser(): User? {
        return songsUser
    }

    fun getSongsUsers(): ArrayList<User>? {
        return usersDashboard?.users
    }

    fun getSongsUserById(id: String): User? {
        return getSongsUsers()?.first { user ->
            user.id == id
        }
    }

    fun syncUsersDashboard(onCompleted: () -> Unit) {
        songsUserService.getRemoteUsers { users, error ->
            if (error != null) {
                println(error)
                this.usersDashboard?.users = ArrayList()
                this.selectedUser = null
            } else if (users != null) {
                users.removeIf { user ->
                    user.id == this.getSongsUser()?.id
                }
                this.usersDashboard?.users = users
                if (selectedUser != null) {
                    this.selectedUser = getSongsUserById(selectedUser!!.id)
                }
            } else {
                println("Didn't receive any users and error")
                this.usersDashboard?.users = ArrayList()
                this.selectedUser = null
            }
            onCompleted()
        }
    }

    fun getRemoteUser(songsAuth: Auth, completion: (User?, Exception?) -> Unit) {
        val currUser = Firebase.auth.currentUser
        if (currUser != null) {
            val userID = currUser.uid
            val user = User(userID, songsAuth.email)
            songsUserService.getRemoteUser(user) { recvUserFromDB, e ->
                if (e != null) {
                    println(e)
                    completion(null, e)
                } else if (recvUserFromDB != null) {
                    completion(recvUserFromDB, null)
                } else {
                    completion(null, Exception("Invalid getRemoteUser form SongsUserFirebaseService closure return"))
                }
            }
        } else {
            completion(null, Exception("Invalid getRemoteUser form SongsUserFirebaseService closure return"))
        }
    }

    fun updateRemoteUserAuth(songsAuth: Auth, error: Exception?, completion: (Exception?) -> Unit) {
        if (error != null) {
            completion(error)
            return
        }
        val currUser = Firebase.auth.currentUser
        if (currUser != null) {
            val userID = currUser.uid
            val user = User(userID, songsAuth.email)
            updateRemoteUser(user, completion)
        } else {
            completion(Exception("Invalid updateRemoteUserAuth form SongsUserFirebaseService closure return"))
            return
        }
    }

    fun updateRemoteUser(user: User, completion: (Exception?) -> Unit) {
        val index = usersDashboard?.users?.indexOfFirst { findUser ->
            findUser.id == user.id
        }
        if (index != null) {
            if (index != -1) {
                usersDashboard?.users?.set(index, user)
            }
        } else {
            usersDashboard?.users?.add(user)
        }

        songsUserService.updateRemoteUser(user) { updatedUser, error ->
            if (error != null) {
                println(error)
                completion(error)
            } else if (updatedUser != null) {
                completion(null)
            } else {
                completion(Exception("Invalid updateRemoteUser form SongsUserFirebaseService closure return"))
            }
        }
    }

    private fun handleFirebaseAuthResponse(songsAuth: Auth, error: Exception?, completion: (Exception?) -> Unit) {
        if (error != null) {
            completion(error)
            return
        }

        initialize(songsAuth) {
            if (this.initialized) {
                this.getRemoteUser(songsAuth) { user, e ->
                    if (e != null) {
                        completion(e)
                    } else {
                        songsUser = user
                        if (user?.isAdmin == true) {
                            syncUsersDashboard() {}
                        }
                    }
                    completion(null)
                }
            } else {
                completion(Exception("Unable to initialize session"))
            }
        }
    }
}