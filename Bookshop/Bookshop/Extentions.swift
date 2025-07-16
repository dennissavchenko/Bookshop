//
//  ColorExtentions.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import SwiftUI

/// Extends `Color` to provide dynamic background and border colors based on order status.
extension Color {
    /// Returns a specific background color based on the provided `OrderStatus`.
    /// - Parameter status: The status of an order.
    /// - Returns: A `Color` instance corresponding to the given order status's background.
    static func getBgColor(for status: OrderStatus) -> Color {
        status == OrderStatus.cancelled ? Color.cancelledBg :
        status == OrderStatus.confirmed ? Color.confirmedBg :
        status == OrderStatus.preparation ? Color.preparationBg :
        status == OrderStatus.shipped ? Color.shippedBg :
        Color.deliveredBg // Default for delivered or any unhandled status
    }

    /// Returns a specific border color based on the provided `OrderStatus`.
    /// - Parameter status: The status of an order.
    /// - Returns: A `Color` instance corresponding to the given order status's border.
    static func getBdColor(for status: OrderStatus) -> Color {
        status == OrderStatus.cancelled ? Color.cancelledBd :
        status == OrderStatus.confirmed ? Color.confirmedBd :
        status == OrderStatus.preparation ? Color.preparationBd :
        status == OrderStatus.shipped ? Color.shippedBd :
        Color.deliveredBd // Default for delivered or any unhandled status
    }
}

/// Extends `Date` to provide common `DateFormatter` instances for various date and time display needs.
extension Date {
    /// Provides a `DateFormatter` for displaying dates in the format "DayOfWeek. Month Day, Year" ("Wed. Jun 11, 2025").
    /// - Returns: A configured `DateFormatter`.
    static func dateFormatter() -> DateFormatter {
        let formatter = DateFormatter()
        formatter.dateFormat = "E. MMM d, yyyy"
        return formatter
    }

    /// Provides a `DateFormatter` for displaying full date and time timestamps in "DD.MM.YYYY | HH:MM" format ("11.06.2025 | 17:02").
    /// - Returns: A configured `DateFormatter`.
    static func timeStampFormatter() -> DateFormatter {
        let formatter = DateFormatter()
        formatter.dateFormat = "dd.MM.yyyy | HH:mm"
        return formatter
    }

    /// Provides a `DateFormatter` for displaying basic dates in "DD.MM.YYYY" format ("11.06.2025").
    /// - Returns: A configured `DateFormatter`.
    static func dateBasicFormatter() -> DateFormatter {
        let formatter = DateFormatter()
        formatter.dateFormat = "dd.MM.yyyy"
        return formatter
    }

    /// Provides a `DateFormatter` for displaying full dates in "Month Day, Year" format ("June 11, 2025").
    /// - Returns: A configured `DateFormatter`.
    static func fullDateFormatter() -> DateFormatter {
        let formatter = DateFormatter()
        formatter.dateFormat = "MMMM d, yyyy"
        return formatter
    }
}

/// Extends `Double` to provide a formatted string representation of a price with a currency symbol.
extension Double {
    /// Formats a `Double` value as a currency string with two decimal places.
    /// - Parameters:
    ///   - price: The numerical price value.
    ///   - currency: The currency symbol to append ("$", "€", "zł").
    /// - Returns: A formatted string representing the price, like "123.45€".
    static func getPriceString(price: Double, currency: String) -> String {
        "\(String(format: "%.2f", price))\(currency)"
    }
}
