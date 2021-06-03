package com.shulga.androidapp.services.extensions

import java.text.DecimalFormat

fun Double.format(fracDigits: Int): String {
    val df = DecimalFormat()
    df.maximumFractionDigits = fracDigits
    return df.format(this)
}