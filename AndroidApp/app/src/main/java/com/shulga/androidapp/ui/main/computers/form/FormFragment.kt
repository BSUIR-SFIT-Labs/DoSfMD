package com.shulga.androidapp.ui.main.computers.form

import android.app.Activity
import android.content.ActivityNotFoundException
import android.content.Intent
import android.net.Uri
import android.os.Bundle
import android.provider.MediaStore
import android.view.*
import android.widget.*
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import com.google.android.gms.maps.model.LatLng
import com.shulga.androidapp.R
import com.shulga.androidapp.models.ComputerAsset
import com.shulga.androidapp.models.MapPoint
import com.shulga.androidapp.services.SessionService
import com.shulga.androidapp.services.extensions.format
import com.shulga.androidapp.services.extensions.startAnimation
import com.shulga.androidapp.services.extensions.stopAnimation
import com.shulga.androidapp.ui.services.googleMapsLocationPicker.GoogleMapsLocationPickerActivity
import com.squareup.picasso.Picasso
import java.util.*

class FormFragment : Fragment() {

    private var assetToEdit: ComputerAsset? = null

    private lateinit var nameEditText: EditText
    private lateinit var descriptionEditText: EditText
    private lateinit var typeEditText: EditText
    private lateinit var processModelEditText: EditText
    private lateinit var ramSizeEditText: EditText
    private lateinit var ssdSizeEditText: EditText
    private lateinit var priceEditText: EditText

    private lateinit var photoImageView: ImageView
    private lateinit var selectPhotoButton: Button
    private lateinit var deletePhotoButton: Button
    private var photoUri: Uri? = null

    private lateinit var videoView: VideoView
    private lateinit var selectVideoButton: Button
    private lateinit var deleteVideoButton: Button
    private var videoUri: Uri? = null

    private lateinit var latitudeText: TextView
    private lateinit var longitudeText: TextView
    private lateinit var releaseNoteEditText: EditText
    private lateinit var releaseInfoStack: LinearLayout
    private lateinit var pickReleaseLocationButton: Button
    private lateinit var deleteReleaseButton: Button
    private var selectedReleasePosition: LatLng? = null

    private lateinit var progressBar: ProgressBar

    override fun onResume() {
        super.onResume()
        syncRelease()
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setHasOptionsMenu(true)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_form, container, false)
        initViewObjects(view)
        setupViewObjects()
        return view
    }

    private fun initViewObjects(view: View) {
        nameEditText = view.findViewById(R.id.fragment_form_name)
        descriptionEditText = view.findViewById(R.id.fragment_form_description)
        typeEditText = view.findViewById(R.id.fragment_form_type)
        processModelEditText = view.findViewById(R.id.fragment_form_processor_model)
        ramSizeEditText = view.findViewById(R.id.fragment_form_ram_size)
        ssdSizeEditText = view.findViewById(R.id.fragment_form_ssd_size)
        priceEditText = view.findViewById(R.id.fragment_form_price)

        photoImageView = view.findViewById(R.id.fragment_form_photo)
        selectPhotoButton = view.findViewById(R.id.fragment_form_button_select_photo)
        deletePhotoButton = view.findViewById(R.id.fragment_form_button_delete_photo)

        videoView = view.findViewById(R.id.fragment_form_video)
        selectVideoButton = view.findViewById(R.id.fragment_form_button_select_video)
        deleteVideoButton = view.findViewById(R.id.fragment_form_button_delete_video)

        latitudeText = view.findViewById(R.id.fragment_form_text_release_latitude)
        longitudeText = view.findViewById(R.id.fragment_form_text_release_longitude)
        releaseNoteEditText = view.findViewById(R.id.fragment_form_tf_release_note)
        releaseInfoStack = view.findViewById(R.id.fragment_form_release_info_stack)
        pickReleaseLocationButton = view.findViewById(R.id.fragment_form_button_pick_location)
        deleteReleaseButton = view.findViewById(R.id.fragment_form_button_delete_release)

        progressBar = view.findViewById(R.id.fragment_form_progress_bar)
    }

    private fun setupViewObjects() {
        setupButtonListeners()

        assetToEdit = SessionService.selectedAsset
        if (assetToEdit != null) {
            nameEditText.setText(assetToEdit!!.name)
            typeEditText.setText(assetToEdit!!.type)
            descriptionEditText.setText(assetToEdit!!.description)
            processModelEditText.setText(assetToEdit!!.processorModel)
            ramSizeEditText.setText(assetToEdit!!.ramSize.toString())
            ssdSizeEditText.setText(assetToEdit!!.ssdSize.toString())
            priceEditText.setText(assetToEdit!!.price.toString())

            val vURL = assetToEdit!!.video?.downloadURL
            if (vURL != null) {
                videoUri = Uri.parse(vURL)
            }

            val pURL = assetToEdit!!.image?.downloadURL
            if (pURL != null) {
                photoUri = Uri.parse(pURL)
            }

            val release = assetToEdit!!.release
            if (release != null) {
                selectedReleasePosition = LatLng(release.latitude, release.longitude)
                releaseNoteEditText.setText(release.note)
            }
        }

        syncPhoto()
        syncVideo()
        syncRelease()
        progressBar.visibility = View.INVISIBLE
    }

    private fun setupButtonListeners() {
        selectPhotoButton.setOnClickListener {
            val i = Intent(Intent.ACTION_PICK, MediaStore.Images.Media.EXTERNAL_CONTENT_URI)
            i.putExtra("crop", "true")
            i.putExtra("aspectX", 100)
            i.putExtra("aspectY", 100)
            i.putExtra("outputX", 200)
            i.putExtra("outputY", 200)

            try {
                i.putExtra("return-data", true)
                startActivityForResult(Intent.createChooser(i, "Select Picture"), 0)
            } catch (ex: ActivityNotFoundException) {
                ex.printStackTrace()
            }
        }

        deletePhotoButton.setOnClickListener {
            photoUri = null
            syncPhoto()
        }

        selectVideoButton.setOnClickListener {
            val intent = Intent(Intent.ACTION_PICK, MediaStore.Video.Media.EXTERNAL_CONTENT_URI)
            intent.type = "video/*"
            startActivityForResult(Intent.createChooser(intent, "Select Video"), 1)
        }

        deleteVideoButton.setOnClickListener {
            videoUri = null
            syncVideo()
        }

        pickReleaseLocationButton.setOnClickListener {
            findNavController().navigate(R.id.action_formFragment_to_googleMapsLocationPickerActivity)
        }

        deleteReleaseButton.setOnClickListener {
            selectedReleasePosition = null
            releaseNoteEditText.setText("")
            syncRelease()
        }
    }

    override fun onCreateOptionsMenu(menu: Menu, inflater: MenuInflater) {
        super.onCreateOptionsMenu(menu, inflater)
        inflater.inflate(R.menu.menu_form_confirm, menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            android.R.id.home -> {
                requireActivity().onBackPressed()
                true
            }
            R.id.menu_form_confirm -> {
                if (validateInput()) {
                    val release: MapPoint?
                    if (selectedReleasePosition != null) {
                        release = MapPoint(
                            releaseNoteEditText.text.toString(),
                            selectedReleasePosition!!.latitude,
                            selectedReleasePosition!!.longitude
                        )
                    } else {
                        release = null
                    }

                    val asset = ComputerAsset(
                        assetToEdit?.id ?: UUID.randomUUID().toString(),
                        nameEditText.text.toString(),
                        typeEditText.text.toString(),
                        descriptionEditText.text.toString(),
                        processModelEditText.text.toString(),
                        ramSizeEditText.text.toString().toInt(),
                        ssdSizeEditText.text.toString().toInt(),
                        priceEditText.text.toString().toDouble(),
                        assetToEdit?.image,
                        assetToEdit?.video,
                        release
                    )

                    startAnimation(progressBar)
                    SessionService.updateRemoteAsset(asset, photoUri, videoUri) { error ->
                        stopAnimation(progressBar)
                        if (error == null) {
                            requireActivity().onBackPressed()
                        }
                    }
                }
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    private fun validateInput(): Boolean {
        var ret = true
        val name = nameEditText.text.toString()
        val code = typeEditText.text.toString()
        val description = descriptionEditText.text.toString()
        val year = processModelEditText.text.toString().toIntOrNull()

        if (name.isEmpty()) {
            nameEditText.error = requireContext().getString(R.string.must_be_not_empty)
            ret = false
        }
        if (code.isEmpty()) {
            typeEditText.error = requireContext().getString(R.string.must_be_not_empty)
            ret = false
        }
        if (description.isEmpty()) {
            descriptionEditText.error = requireContext().getString(R.string.must_be_not_empty)
            ret = false
        }
        if (year == null) {
            processModelEditText.error = requireContext().getString(R.string.must_be_not_empty)
            ret = false
        }
        if (selectedReleasePosition != null && releaseNoteEditText.text.toString().isEmpty()) {
            releaseNoteEditText.error = requireContext().getString(R.string.must_be_not_empty)
            ret = false
        }
        return ret
    }

    override fun onActivityResult(requestCode: Int, resultCode: Int, data: Intent?) {
        super.onActivityResult(requestCode, resultCode, data)
        if (requestCode == 0 && resultCode == Activity.RESULT_OK) {
            try {
                if (data != null) {
                    photoUri = data.data
                    syncPhoto()
                }
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }
        if (requestCode == 1 && resultCode == Activity.RESULT_OK) {
            try {
                if (data != null) {
                    videoUri = data.data
                    syncVideo()
                }
            } catch (e: Exception) {
                e.printStackTrace()
            }
        }
    }

    private fun syncPhoto() {
        if (photoUri != null) {
            Picasso.get()
                .load(photoUri)
                .placeholder(R.drawable.ic_empty)
                .error(R.drawable.ic_empty)
                .into(photoImageView)
            photoImageView.visibility = View.VISIBLE
        } else {
            photoImageView.visibility = View.GONE
        }
    }

    private fun syncVideo() {
        if (videoUri != null) {
            videoView.setVideoURI(videoUri)
//            videoView.setMediaController(MediaController(requireContext()))
//            videoView.seekTo(100)
            videoView.visibility = View.VISIBLE
        } else {
            videoView.visibility = View.GONE
        }
    }

    private fun syncRelease() {
        val newPos = GoogleMapsLocationPickerActivity.lastPickedLocation
        if (newPos != null) {
            selectedReleasePosition = GoogleMapsLocationPickerActivity.lastPickedLocation
            GoogleMapsLocationPickerActivity.lastPickedLocation = null
        }

        val pos = selectedReleasePosition
        if (pos != null) {
            latitudeText.text = pos.latitude.format(4)
            longitudeText.text = pos.longitude.format(4)
            releaseInfoStack.visibility = View.VISIBLE
        } else {
            releaseInfoStack.visibility = View.GONE
        }
    }
}