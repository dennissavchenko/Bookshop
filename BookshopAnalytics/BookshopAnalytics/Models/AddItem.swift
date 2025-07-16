//
//  SimpleOrder.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 09/06/2025.
//

import Foundation
import SwiftUI

/// Represents a generic item available in the bookshop, including books, magazines, and newspapers.
///
/// This struct is designed to be `Identifiable` for use in SwiftUI lists, and `Decodable`
/// to allow for easy parsing from JSON data received from an API.
struct AddItem: Identifiable, Decodable {
    var id: Int
    var name: String
    var description: String
    var imageUrl: String
    var publishingDate: Date
    var language: String?
    var price: String
    var amountInStock: String
    var publisherId: Int?
    var ageCategoryId: Int?
    var itemType: ItemType?
    var authorsIds: [Int]
    var genresIds: [Int]
    var numberOfPages: String
    var coverType: Int?
    var isSpecialEdition: Bool?
    var headline: String
    var topics: [String]?
    var isUsed: Bool
    var isSealed: Bool?
    var condition: Int?
    var hasAnnotations: Bool?
}

