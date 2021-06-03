package com.shulga.androidapp.ui.main.blocked

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.Button
import androidx.activity.addCallback
import androidx.fragment.app.Fragment
import com.shulga.androidapp.R
import com.shulga.androidapp.services.SessionService

class BlockedFragment : Fragment() {
    private lateinit var logOutButton: Button

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        val view = inflater.inflate(R.layout.fragment_blocked, container, false)
        initViewObjects(view)
        setupViewObjects()
        return view
    }

    private fun initViewObjects(view: View) {
        logOutButton = view.findViewById(R.id.fragment_blocked_button_log_out)
    }

    private fun setupViewObjects() {
        setupButtonListeners()
    }

    private fun setupButtonListeners() {
        requireActivity().onBackPressedDispatcher.addCallback(viewLifecycleOwner) {
            requireActivity().finishAffinity()
        }

        logOutButton.setOnClickListener {
            SessionService.destroy()
            requireActivity().finish()
        }
    }
}