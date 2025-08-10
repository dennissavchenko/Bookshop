//
//  Extentions.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 24/06/2025.
//

import SwiftUI

extension String {
    var isEmptyW: Bool {
        self.trimmingCharacters(in: .whitespaces).isEmpty
    }
    var countW: Int {
        self.trimmingCharacters(in: .whitespaces).count
    }
    func validatePriceString() -> Bool {
        let regex = try! Regex(#"^(0|[1-9]\d*)(\.\d{0,2})?$"#)
        return self.contains(regex)
    }
    func validateAmountString() -> Bool {
        let regex = try! Regex(#"^(0|[1-9]\d*)$"#)
        return self.contains(regex)
    }
}

extension Double {
    var priceFormat: String {
        "$\(String(format: "%.2f", self))"
    }
    var ratingFormat: String {
        String(format: "%.1f", self)
    }
}

extension Date {
    static func basicDateFormatter() -> DateFormatter {
        let formatter = DateFormatter()
        formatter.dateFormat = "MMMM d, yyyy"
        return formatter
    }
    func customFormatDateFormatter(format: String) -> String {
        let formatter = DateFormatter()
        formatter.dateFormat = format
        return formatter.string(from: self)
    }
}

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


