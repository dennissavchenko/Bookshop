//
//  CoverType.swift
//  BookShop
//
//  Created by dennis savchenko on 10/06/2025.
//

/// Represents the cover type of a book.
///
/// This enum conforms to `String`, `Decodable`, and `CaseIterable`
/// to facilitate easy serialization/deserialization and iteration.
enum CoverType: String, Decodable, CaseIterable {
    case soft = "Soft"
    case hard = "Hard"
    case spiralBound = "SpiralBound"
    
    var intValue: Int {
        switch self {
        case .hard: return 0
        case .soft: return 1
        case .spiralBound: return 2
        }
    }
    
}
