package com.shulga.androidapp.services

import com.shulga.androidapp.models.auth.Auth

object AuthStorage {
    private val preferences = SharedPreferencesService.encrypted

    fun save(songsAuth: Auth) {
        with(preferences.edit()) {
            putString(Constants.SharedPreferences.Encrypted.EMAIL_KEY, songsAuth.email)
            putString(Constants.SharedPreferences.Encrypted.PASSWORD_KEY, songsAuth.password)
            commit()
        }
    }

    fun delete() {
        with(preferences.edit()) {
            remove(Constants.SharedPreferences.Encrypted.EMAIL_KEY)
            remove(Constants.SharedPreferences.Encrypted.PASSWORD_KEY)
            commit()
        }
    }

    fun restore(): Auth? {
        val email = preferences.getString(Constants.SharedPreferences.Encrypted.EMAIL_KEY, null)
        val password = preferences.getString(Constants.SharedPreferences.Encrypted.PASSWORD_KEY, null)
        if (email != null && password != null) {
            return Auth(email, password)
        }
        return null
    }
}