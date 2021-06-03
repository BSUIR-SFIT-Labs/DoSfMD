package com.shulga.androidapp.ui.main.computers

import android.content.Context
import android.view.LayoutInflater
import android.view.View
import android.view.ViewGroup
import android.widget.BaseAdapter
import android.widget.TextView
import com.shulga.androidapp.R
import com.shulga.androidapp.models.ComputerAsset
import com.squareup.picasso.Picasso
import de.hdodenhof.circleimageview.CircleImageView

class ComputersAdapter (private val context: Context, private val assets: ArrayList<ComputerAsset>): BaseAdapter() {

    private val inflater: LayoutInflater = context.getSystemService(Context.LAYOUT_INFLATER_SERVICE) as LayoutInflater

    override fun getCount(): Int {
        return assets.count()
    }

    override fun getItem(position: Int): Any {
        return assets[position]
    }

    override fun getItemId(position: Int): Long {
        return assets[position].hashCode().toLong()
    }

    override fun getView(position: Int, convertView: View?, parent: ViewGroup?): View {
        val view: View
        if (convertView == null) {
            view = inflater.inflate(R.layout.element_computer_cell, parent, false)
        } else {
            view = convertView
        }

        val asset: ComputerAsset = assets[position]

        updateCellView(view, asset)

        return view
    }

    private fun updateCellView(view: View, p: ComputerAsset) {
        val photo: CircleImageView = view.findViewById(R.id.element_computer_cell_photo)
        val name: TextView = view.findViewById(R.id.element_computer_cell_text_name)
        val description: TextView = view.findViewById(R.id.element_computer_cell_text_description)
        val price: TextView = view.findViewById(R.id.element_computer_cell_text_price)

        if (p.image != null) {
            Picasso.get()
                .load(p.image!!.downloadURL)
                .placeholder(R.drawable.ic_empty)
                .error(R.drawable.ic_empty)
                .into(photo)
        }

        name.text = p.name
        description.text = p.description
        price.text = p.price.toString()

    }
}