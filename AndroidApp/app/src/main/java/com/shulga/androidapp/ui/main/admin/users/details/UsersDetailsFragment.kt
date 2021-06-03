package com.shulga.androidapp.ui.main.admin.users.details

import android.app.AlertDialog
import android.os.Bundle
import android.view.LayoutInflater
import android.view.MenuItem
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import android.widget.TextView
import androidx.fragment.app.Fragment
import com.shulga.androidapp.R
import com.shulga.androidapp.services.SessionService
import java.lang.Exception

class UsersDetailsFragment : Fragment() {
    private lateinit var emailTextView: TextView
    private lateinit var statusTextView: TextView

    private lateinit var activeButton: Button
    private lateinit var blockButton: Button

    override fun onResume() {
        super.onResume()
        syncWithSelected()

        if (SessionService.selectedUser == null) {
            requireActivity().onBackPressed()
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setHasOptionsMenu(true)
    }

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        val view = inflater.inflate(R.layout.fragment_users_details, container, false)
        initViewObjects(view)
        setupViewObjects()
        return view
    }

    private fun initViewObjects(view: View) {
        emailTextView = view.findViewById(R.id.fragment_users_details_textview_email_value)
        statusTextView = view.findViewById(R.id.fragment_users_details_textview_status_value)

        activeButton = view.findViewById(R.id.fragment_users_details_button_active)
        blockButton = view.findViewById(R.id.fragment_users_details_button_blocked)
    }

    private fun setupViewObjects() {
        setupButtonListeners()
    }

    private fun setupButtonListeners() {
        activeButton.setOnClickListener {
            if (SessionService.selectedUser != null) {
                SessionService.selectedUser?.isBlocked = false
                SessionService.updateRemoteUser(SessionService.selectedUser!!) { error ->
                    handleChangeResult(error)
                }
            }
        }
        blockButton.setOnClickListener {
            if (SessionService.selectedUser != null) {
                SessionService.selectedUser?.isBlocked = true
                SessionService.updateRemoteUser(SessionService.selectedUser!!) { error ->
                    handleChangeResult(error)
                }
            }
        }
    }

    private fun handleChangeResult(error: Exception?) {
        var title: Int = R.string.success
        var msg: Int = R.string.success_change
        if (error != null) {
            println(error)
            title = R.string.error
            msg = R.string.something_went_wrong
        }
        showAlert(title, msg)
        syncWithSelected()
    }

    private fun showAlert(title: Int, msg: Int) {
        val alertDialog: AlertDialog.Builder = AlertDialog.Builder(requireContext())
        alertDialog.setTitle(title)
        alertDialog.setMessage(msg)
        alertDialog.setPositiveButton("OK") { dialog, id ->
            dialog.dismiss()
        }
        val alert = alertDialog.create()
        alert.show()
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            android.R.id.home -> {
                requireActivity().onBackPressed()
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    private fun syncWithSelected() {
        val user = SessionService.selectedUser
        if (user != null) {
            emailTextView.setText(user.email)
            if (user.isBlocked) {
                statusTextView.setText(R.string.blocked)
            } else {
                statusTextView.setText(R.string.active)
            }
        }
    }
}