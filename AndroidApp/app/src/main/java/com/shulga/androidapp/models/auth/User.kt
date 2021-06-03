package com.shulga.androidapp.models.auth

data class User (
    val id: String,
    val email: String,
    var isAdmin: Boolean = false,
    var isBlocked: Boolean = false
)