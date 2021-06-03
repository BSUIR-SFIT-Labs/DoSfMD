package com.shulga.androidapp.ui.main.computers.details

import android.os.Bundle
import com.shulga.androidapp.R
import com.shulga.androidapp.ui.Activity

class ComputerDetailsActivity : Activity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_computer_details)

        supportActionBar?.setDisplayHomeAsUpEnabled(true)
        supportActionBar?.title = getString(R.string.details)
    }
}