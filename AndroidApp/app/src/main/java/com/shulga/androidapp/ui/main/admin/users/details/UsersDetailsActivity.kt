package com.shulga.androidapp.ui.main.admin.users.details

import android.os.Bundle
import com.shulga.androidapp.R
import com.shulga.androidapp.ui.Activity

class UsersDetailsActivity : Activity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setContentView(R.layout.activity_users_details)

        supportActionBar?.setDisplayHomeAsUpEnabled(true)
        supportActionBar?.title = getString(R.string.user_details)
    }
}