//
//  ItemType.swift
//  BookShop
//
//  Created by dennis savchenko on 10/06/2025.
//

/// Represents the type of an item.
///
/// This enum conforms to `String`, `Decodable`, and `CaseIterable`
/// to facilitate easy serialization/deserialization and iteration.
enum ItemType: String, Decodable, CaseIterable, Hashable {
    case book = "Book"
    case magazine = "Magazine"
    case newspaper = "Newspaper"
    case typeless = ""
}
