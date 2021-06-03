package com.shulga.androidapp.ui.services.googleMapsLocationPicker

import android.os.Bundle
import androidx.appcompat.app.AppCompatActivity
import com.google.android.gms.maps.model.LatLng
import com.shulga.androidapp.R

class GoogleMapsLocationPickerActivity : AppCompatActivity() {

    // Change to activity result
    companion object {
        var lastPickedLocation: LatLng? = null
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_google_maps_location_picker)

        supportActionBar?.setDisplayHomeAsUpEnabled(true)
        supportActionBar?.title = getString(R.string.pick_location)
    }
}