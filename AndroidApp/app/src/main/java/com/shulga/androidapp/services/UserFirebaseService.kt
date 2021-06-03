package com.shulga.androidapp.services

import com.google.firebase.firestore.FirebaseFirestore
import com.shulga.androidapp.models.auth.User

class UserFirebaseService {
    private val db = FirebaseFirestore.getInstance()

    fun deleteRemoteUser(user: User, completion: (Exception?) -> Unit) {
        val document = db.collection(Constants.Api.Firebase.usersCollectionFirebaseName).document(user.id)
        document
            .delete()
            .addOnSuccessListener {
                completion(null)
            }
            .addOnFailureListener { e ->
                completion(e)
            }
    }

    private fun uploadUser(user: User, completion: (Exception?) -> Unit) {
        try {
            val map = GsonConverter.toMap(user)?.toMutableMap()
            map?.remove("id")

            if (map != null) {
                val document = db.collection(Constants.Api.Firebase.usersCollectionFirebaseName).document(user.id)
                document
                    .set(map)
                    .addOnSuccessListener {
                        println("Document successfully written!")
                        completion(null)
                    }
                    .addOnFailureListener { e ->
                        println("Error writing document: $e")
                        completion(e)
                    }
            } else {
                completion(Exception("Unable to create json object"))
            }
        } catch (_: Throwable) {
            completion(Exception("Unable to encode song asset"))
        }
    }

    private fun updateRemoteUserRec(user: User, completion: (User?, Exception?) -> Unit) {
        uploadUser(user) { error ->
            if (error != null) {
                completion(null, error)
            } else {
                completion(user, null)
            }
        }
    }

    fun updateRemoteUser(user: User, completion: (User?, Exception?) -> Unit) {
        updateRemoteUserRec(user) { updatedUser, error ->
            completion(updatedUser, error)
        }
    }

    fun getRemoteUser(user: User, completion: (User?, Exception?) -> Unit) {
        val docRef = db.collection(Constants.Api.Firebase.usersCollectionFirebaseName).document(user.id)
        docRef.get()
            .addOnSuccessListener { document ->
                var recvUser: User? = user
                if (document != null) {
                    val jsonData = document.data
                    jsonData?.set("id", document.id)
                    recvUser = GsonConverter.toObj(GsonConverter.toStr(jsonData))
                    if (recvUser == null) {
                        completion(null, Exception("Can't convert Firebase data to SongsUser"))
                    }
                } else {
                    completion(null, Exception("Both doc and error in getRemoteUser are nil"))
                }
                completion(recvUser, null)
            }
            .addOnFailureListener { e ->
                completion(null, e)
            }
    }

    fun getRemoteUsers(completion: (ArrayList<User>?, Exception?) -> Unit) {
        db.collection(Constants.Api.Firebase.usersCollectionFirebaseName)
            .get()
            .addOnSuccessListener { query ->
                val users: ArrayList<User> = ArrayList()
                query.documents.forEach { document ->
                    val jsonData = document.data
                    jsonData?.set("id", document.id)

                    val user: User? = GsonConverter.toObj(GsonConverter.toStr(jsonData))
                    if (user != null) {
                        users.add(user)
                    } else {
                        println("Can't convert Firebase data to SongsUser")
                    }
                }
                completion(users, null)
            }
            .addOnFailureListener { e ->
                completion(null, e)
            }
    }
}