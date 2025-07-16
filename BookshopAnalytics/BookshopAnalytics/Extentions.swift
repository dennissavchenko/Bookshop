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
}


