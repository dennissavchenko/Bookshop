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
        "\(String(format: "%.2f", self))$"
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


