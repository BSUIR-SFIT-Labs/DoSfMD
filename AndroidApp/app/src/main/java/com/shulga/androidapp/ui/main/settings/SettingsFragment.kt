package com.shulga.androidapp.ui.main.settings

import android.os.Bundle
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.*
import android.view.*
import androidx.activity.addCallback
import androidx.fragment.app.Fragment
import com.shulga.androidapp.App
import com.shulga.androidapp.R
import com.shulga.androidapp.services.Constants
import com.shulga.androidapp.services.SessionService
import com.shulga.androidapp.services.SharedPreferencesService
import com.shulga.androidapp.services.TypefaceService
import com.shulga.androidapp.ui.Activity
import com.zeugmasolutions.localehelper.Locales

class SettingsFragment : Fragment() {
    private lateinit var languageSystemButton: Button
    private lateinit var languageEnglishButton: Button
    private lateinit var languageRussianButton: Button

    private lateinit var fontSpinner: Spinner
    private lateinit var availableFonts: Array<String?>

    private lateinit var logOutButton: Button

    override fun onCreateView(inflater: LayoutInflater, container: ViewGroup?, savedInstanceState: Bundle?): View? {
        val view = inflater.inflate(R.layout.fragment_settings, container, false)
        initViewObjects(view)
        setupViewObjects()
        return view
    }

    private fun initViewObjects(view: View) {
        languageSystemButton = view.findViewById(R.id.fragment_settings_button_language_system)
        languageEnglishButton = view.findViewById(R.id.fragment_settings_button_language_english)
        languageRussianButton = view.findViewById(R.id.fragment_settings_button_language_russian)

        initSpinnerObject(view)

        logOutButton = view.findViewById(R.id.fragment_settings_button_log_out)
    }

    private fun initSpinnerObject(view: View) {
        fontSpinner = view.findViewById(R.id.fragment_settings_spinner_font)
        availableFonts = TypefaceService.getAvailableFonts(requireContext().assets, "fonts")
        val adapter = ArrayAdapter(requireContext(), R.layout.support_simple_spinner_dropdown_item, availableFonts) // TODO:
        adapter.setDropDownViewResource(android.R.layout.simple_dropdown_item_1line)
        fontSpinner.adapter = adapter
        val fontName: String? = SharedPreferencesService.standard.getString(Constants.SharedPreferences.Standard.FONT_KEY, "")
        if (!fontName.isNullOrBlank()) {
            fontSpinner.setSelection(availableFonts.indexOf(fontName))
        }
    }

    private fun setupViewObjects() {
        setupButtonListeners()
        setupSpinnerListeners()
    }

    private fun setupButtonListeners() {
        requireActivity().onBackPressedDispatcher.addCallback(viewLifecycleOwner) {
            requireActivity().finishAffinity()
        }

        languageSystemButton.setOnClickListener {
            (requireActivity() as Activity).updateLocale(App.systemLocale())
        }

        languageEnglishButton.setOnClickListener {
            (requireActivity() as Activity).updateLocale(Locales.English)
        }

        languageRussianButton.setOnClickListener {
            (requireActivity() as Activity).updateLocale(Locales.Russian)
        }

        logOutButton.setOnClickListener {
            SessionService.destroy()
            requireActivity().finish()
        }
    }

    private fun setupSpinnerListeners() {
        fontSpinner.onItemSelectedListener = object: AdapterView.OnItemSelectedListener{
            override fun onItemSelected(parent: AdapterView<*>, view: View, position: Int, id: Long) {
                val fontName: String = parent.getItemAtPosition(position).toString()
                TypefaceService.overrideAllFontFields(requireContext(), "fonts/" + fontName)
                with(SharedPreferencesService.standard.edit()) {
                    putString(Constants.SharedPreferences.Standard.FONT_KEY, fontName)
                    apply()
                }
            }

            override fun onNothingSelected(parent: AdapterView<*>) {
            }
        }
    }
}