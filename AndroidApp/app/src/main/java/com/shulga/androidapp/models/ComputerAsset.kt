package com.shulga.androidapp.models

data class ComputerAsset (
    val id: String,
    var name: String,
    var description: String,
    var type: String,
    var processorModel: String,
    var ramSize: Int,
    var ssdSize: Int,
    var price: Double,

    var image: FileData?,
    var video: FileData?,

    var release: MapPoint?
)