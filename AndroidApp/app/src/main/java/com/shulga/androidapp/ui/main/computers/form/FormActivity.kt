package com.shulga.androidapp.ui.main.computers.form

import android.content.Intent
import android.os.Bundle
import com.shulga.androidapp.R
import com.shulga.androidapp.ui.Activity

class FormActivity : Activity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_form)

        supportActionBar?.setDisplayHomeAsUpEnabled(true)
        supportActionBar?.title = getString(R.string.new_computer)
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
    }
}