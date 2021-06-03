package com.shulga.androidapp.services

import android.net.Uri
import com.google.firebase.firestore.FirebaseFirestore
import com.google.firebase.storage.FirebaseStorage
import com.google.firebase.storage.StorageMetadata
import com.google.firebase.storage.StorageReference
import com.shulga.androidapp.App
import com.shulga.androidapp.models.ComputerAsset
import com.shulga.androidapp.models.FileData
import java.io.InputStream
import java.util.*
import kotlin.collections.ArrayList

class ComputerAssetFirebaseService {

    private val db = FirebaseFirestore.getInstance()
    private val storage = FirebaseStorage.getInstance()

    fun deleteRemoteAsset(asset: ComputerAsset, completion: (Exception?) -> Unit) {

        val photoFile = asset.image
        if (photoFile != null) {
            deleteFile(photoFile) { error ->
                if (error != null) {
                    println(error)
                } else {
                    println("Deleted file with path $photoFile.path")
                }
            }
        }

        val videoFile = asset.video
        if (videoFile != null) {
            deleteFile(videoFile) { error ->
                if (error != null) {
                    println(error)
                } else {
                    println("Deleted file with path $videoFile.path")
                }
            }
        }

        val document = db.collection(Constants.Api.Firebase.assetsCollectionName).document(asset.id)
        document
            .delete()
            .addOnSuccessListener {
                completion(null)
            }
            .addOnFailureListener { e ->
                completion(e)
            }
    }

    private fun getStorageDownloadURL(path: String, completion: (Uri?, Exception?) -> Unit) {
        val storageRef = storage.reference
        val fileRef = storageRef.child(path)
        fileRef
            .downloadUrl
            .addOnSuccessListener { uri ->
                completion(uri, null)
            }
            .addOnFailureListener { e ->
                completion(null, e)
            }
    }

    private fun deleteFile(file: FileData, completion: (Exception?) -> Unit) {
        val storageRef = this.storage.reference
        val fileRef = storageRef.child(file.path)

        fileRef
            .delete()
            .addOnSuccessListener {
                completion(null)
            }
            .addOnFailureListener { e ->
                completion(e)
            }
    }

    private fun uploadFile(fileRef: StorageReference, stream: InputStream, metadata: StorageMetadata, completion: (FileData?, Exception?) -> Unit) {
        fileRef
            .putStream(stream, metadata)
            .addOnSuccessListener { _ ->
                this.getStorageDownloadURL(fileRef.path) { uri, error ->
                    when {
                        error != null -> {
                            completion(null, error)
                        }
                        uri != null -> {
                            println("Uploaded file $fileRef.path")
                            completion(FileData(fileRef.path, uri.toString()), null)
                        }
                        else -> {
                            completion(null, Exception("Both uri and error in uploadFile are null"))
                        }
                    }
                }
            }
            .addOnFailureListener { e ->
                completion(null, e)
            }
    }

    private fun uploadImage(imageUri: Uri, completion: (FileData?, Exception?) -> Unit) {
        try {
            val inputStream: InputStream? = App.applicationContext().contentResolver.openInputStream(imageUri)
            if (inputStream != null) {
                val storageRef = this.storage.reference
                val path = "${Constants.Api.Firebase.imagesFolderName}/${UUID.randomUUID()}-ai"
                val videoRef = storageRef.child(path)
                val metadata = StorageMetadata()
                this.uploadFile(videoRef, inputStream, metadata) { fileData, error ->
                    completion(fileData, error)
                }
            } else {
                completion(null, Exception("Unable to open stream from Uri"))
            }
        } catch (error: Throwable) {
            completion(null, Exception("Unable to process image Uri"))
        }
    }

    private fun uploadVideo(videoUri: Uri, completion: (FileData?, Exception?) -> Unit) {
        try {
            val inputStream: InputStream? = App.applicationContext().contentResolver.openInputStream(videoUri)
            if (inputStream != null) {
                val storageRef = this.storage.reference
                val path = "${Constants.Api.Firebase.videosFolderName}/${UUID.randomUUID()}-av"
                val videoRef = storageRef.child(path)
                val metadata = StorageMetadata()
                this.uploadFile(videoRef, inputStream, metadata) { fileData, error ->
                    completion(fileData, error)
                }
            } else {
                completion(null, Exception("Unable to open stream from Uri"))
            }
        } catch (error: Throwable) {
            completion(null, Exception("Unable to process video Uri"))
        }
    }

    private fun uploadAsset(asset: ComputerAsset, completion: (Exception?) -> Unit) {
        try {
            val map = GsonConverter.toMap(asset)?.toMutableMap()
            map?.remove("id")

            if (map != null) {
                val document = db.collection(Constants.Api.Firebase.assetsCollectionName).document(asset.id)
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

    private fun updateRemoteAssetRec(asset: ComputerAsset, photoUri: Uri?, videoUri: Uri?, completion: (ComputerAsset?, Exception?) -> Unit) {

        // Upload files 1 by 1 with every calls
        // Priority:
        // 1 - Video
        // 2 - Image
        // 3 - Asset

        if (asset.video?.downloadURL != videoUri?.toString()) {
            var updatedAsset = asset

            // Delete previous file in background
            val videoFileData = asset.video
            if (videoFileData != null) {
                updatedAsset.video = null
                deleteFile(videoFileData) { error ->
                    if (error != null) {
                        println(error)
                    } else {
                        println("Deleted photo with path $videoFileData.path")
                    }
                }
            }

            // Upload with recursive call in completion
            if (videoUri != null) {
                uploadVideo(videoUri) { fileData, error ->
                    if (error != null) {
                        // Error uploading video so we ignore it
                        println(error)
                        this.updateRemoteAssetRec(updatedAsset, photoUri, null, completion)
                    } else if (fileData != null) {
                        // Successful video upload
                        updatedAsset.video = fileData

                        val downloadUri = Uri.parse(fileData.downloadURL)
                        this.updateRemoteAssetRec(updatedAsset, photoUri, downloadUri, completion)
                    }
                }
            } else {
                this.updateRemoteAssetRec(updatedAsset, photoUri, videoUri, completion)
            }

            return
        }


        if (asset.image?.downloadURL != photoUri?.toString()) {
            var updatedAsset = asset

            // Delete previous file in background
            val photoFileData = asset.image
            if (photoFileData != null) {
                updatedAsset.image = null
                deleteFile(photoFileData) { error ->
                    if (error != null) {
                        println(error)
                    } else {
                        println("Deleted photo with path $photoFileData.path")
                    }
                }
            }

            // Upload with recursive call in completion
            if (photoUri != null) {
                uploadImage(photoUri) { fileData, error ->
                    if (error != null) {
                        // Error uploading video so we ignore it
                        println(error)
                        this.updateRemoteAssetRec(updatedAsset, null, videoUri, completion)
                    } else if (fileData != null) {
                        // Successful video upload
                        updatedAsset.image = fileData

                        val downloadUri = Uri.parse(fileData.downloadURL)
                        this.updateRemoteAssetRec(updatedAsset, downloadUri, videoUri, completion)
                    }
                }
            } else {
                this.updateRemoteAssetRec(updatedAsset, photoUri, videoUri, completion)
            }

            return
        }

        uploadAsset(asset) { error ->
            if (error != null) {
                completion(null, error)
            } else {
                completion(asset, null)
            }
        }

    }

    fun updateRemoteAsset(asset: ComputerAsset, photoUri: Uri?, videoUri: Uri?, completion: (ComputerAsset?, Exception?) -> Unit) {
        updateRemoteAssetRec(asset, photoUri, videoUri) { updatedAsset, error ->
            completion(updatedAsset, error)
        }
    }

    fun getRemoteAssets(completion: (ArrayList<ComputerAsset>?, Exception?) -> Unit) {
        db.collection(Constants.Api.Firebase.assetsCollectionName)
            .get()
            .addOnSuccessListener { query ->
                val assets: ArrayList<ComputerAsset> = ArrayList()
                query.documents.forEach { document ->
                    val jsonData = document.data
                    jsonData?.set("id", document.id)

                    val asset: ComputerAsset? = GsonConverter.toObj(GsonConverter.toStr(jsonData))
                    if (asset != null) {
                        assets.add(asset)
                    } else {
                        println("Can't convert Firebase data to SongsAsset")
                    }
                }
                completion(assets, null)
            }
            .addOnFailureListener { e ->
                completion(null, e)
            }
    }
}