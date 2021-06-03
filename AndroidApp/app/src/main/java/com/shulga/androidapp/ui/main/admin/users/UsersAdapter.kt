package com.shulga.androidapp.ui.main.admin.users

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.TextView
import com.shulga.androidapp.R
import com.shulga.androidapp.models.auth.User

class UsersAdapter (private val context: Context, private val users: ArrayList<User>): BaseAdapter() {

    private val inflater: LayoutInflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

    override fun getCount(): Int {
        return users.count()
    }

    override fun getItem(position: Int): Any {
        return users[position]
    }

    override fun getItemId(position: Int): Long {
        return users[position].hashCode().toLong()
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        val view: View
        if (convertView == null) {
            view = inflater.inflate(R.layout.element_users_cell, parent, false)
        } else {
            view = convertView
        }

        val user: User = users[position]

        updateCellView(view, user)

        return view
    }

    private fun updateCellView(view: View, user: User) {
        val email: TextView = view.findViewById(R.id.element_users_cell_text_email)

        email.text = user.email
    }
}