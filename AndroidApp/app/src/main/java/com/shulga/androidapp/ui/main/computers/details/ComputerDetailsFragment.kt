package com.shulga.androidapp.ui.main.computers.details

import android.os.Bundle
import android.view.*
import android.widget.*
import androidx.fragment.app.Fragment
import androidx.navigation.fragment.findNavController
import com.shulga.androidapp.R
import com.shulga.androidapp.services.SessionService
import com.shulga.androidapp.services.extensions.format
import com.squareup.picasso.Picasso

class ComputerDetailsFragment : Fragment() {
    private lateinit var photoView: ImageView
    private lateinit var videoView: VideoView

    private lateinit var nameTextView: TextView
    private lateinit var descriptionTextView: TextView
    private lateinit var typeTextView: TextView
    private lateinit var processorModelTextView: TextView
    private lateinit var ramSizeTextView: TextView
    private lateinit var ssdSizeTextView: TextView
    private lateinit var priceTextView: TextView

    private lateinit var videoStackView: LinearLayout
    private lateinit var releaseLatitudeTextView: TextView
    private lateinit var releaseLongitudeTextView: TextView
    private lateinit var releaseNoteTextView: TextView
    private lateinit var releaseStackView: LinearLayout

    override fun onResume() {
        super.onResume()
        syncWithSelected()

        if (SessionService.selectedAsset == null) {
            requireActivity().onBackPressed()
        }
    }

    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        setHasOptionsMenu(true)
    }

    override fun onCreateView(
        inflater: LayoutInflater, container: ViewGroup?,
        savedInstanceState: Bundle?
    ): View? {
        val view = inflater.inflate(R.layout.fragment_computer_details, container, false)
        initViewObjects(view)
        setupViewObjects()
        return view
    }

    private fun initViewObjects(view: View) {
        photoView = view.findViewById(R.id.fragment_computer_details_photo)
        nameTextView = view.findViewById(R.id.fragment_computer_details_text_name)
        descriptionTextView = view.findViewById(R.id.fragment_computer_details_text_description)
        typeTextView = view.findViewById(R.id.fragment_computer_details_text_type)
        processorModelTextView = view.findViewById(R.id.fragment_computer_details_text_processorModel)
        ramSizeTextView = view.findViewById(R.id.fragment_computer_details_text_ramSize)
        ssdSizeTextView = view.findViewById(R.id.fragment_computer_details_text_ssdSize)
        priceTextView = view.findViewById(R.id.fragment_computer_details_text_price)
        videoView = view.findViewById(R.id.fragment_songs_details_video)
        videoStackView = view.findViewById(R.id.fragment_songs_details_video_stack)
        releaseLatitudeTextView = view.findViewById(R.id.fragment_songs_details_text_release_latitude)
        releaseLongitudeTextView = view.findViewById(R.id.fragment_songs_details_text_release_longitude)
        releaseNoteTextView = view.findViewById(R.id.fragment_songs_details_text_release_note)
        releaseStackView = view.findViewById(R.id.fragment_songs_details_release_stack)
    }

    private fun setupViewObjects() {
        videoView.setMediaController(MediaController(requireContext()))
    }

    override fun onCreateOptionsMenu(menu: Menu, inflater: MenuInflater) {
        super.onCreateOptionsMenu(menu, inflater)
        inflater.inflate(R.menu.menu_computer_details_edit, menu)
    }

    override fun onOptionsItemSelected(item: MenuItem): Boolean {
        return when (item.itemId) {
            android.R.id.home -> {
                requireActivity().onBackPressed()
                true
            }
            R.id.menu_computer_details_edit -> {
                findNavController().navigate(R.id.action_songsDetailsFragment_to_formActivity2)
                true
            }
            else -> super.onOptionsItemSelected(item)
        }
    }

    private fun syncWithSelected() {
        val asset = SessionService.selectedAsset
        if (asset != null) {
            if (asset.image != null) {
                Picasso.get()
                    .load(asset.image!!.downloadURL)
                    .placeholder(R.drawable.ic_empty)
                    .error(R.drawable.ic_empty)
                    .into(photoView)
                photoView.visibility = View.VISIBLE
            } else {
                photoView.visibility = View.GONE
            }

            nameTextView.text = asset.name
            descriptionTextView.text = asset.description
            typeTextView.text = asset.type
            processorModelTextView.text = asset.processorModel
            ramSizeTextView.text = asset.ramSize.toString()
            ssdSizeTextView.text = asset.ssdSize.toString()
            priceTextView.text = asset.price.toString()

            if (asset.video != null) {
                videoView.setVideoPath(asset.video!!.downloadURL)
                videoView.seekTo(100)
                videoStackView.visibility = View.VISIBLE
            } else {
                videoStackView.visibility = View.GONE
            }

            if (asset.release != null) {
                releaseLatitudeTextView.text = asset.release!!.latitude.format(4)
                releaseLongitudeTextView.text = asset.release!!.longitude.format(4)
                releaseNoteTextView.text = asset.release!!.note
                releaseStackView.visibility = View.VISIBLE
            } else {
                releaseStackView.visibility = View.GONE
            }
        }
    }
}