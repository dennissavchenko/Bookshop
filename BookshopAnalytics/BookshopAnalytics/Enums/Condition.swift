//
//  Condition.swift
//  BookShop
//
//  Created by dennis savchenko on 10/06/2025.
//

/// Represents the physical condition of a used item.
///
/// This enum conforms to `String`, `Decodable`, and `CaseIterable`
/// to facilitate easy serialization/deserialization and iteration.
enum Condition: String, Decodable, CaseIterable {
    case mint = "Mint"
    case good = "Good"
    case fair = "Fair"
    case poor = "Poor"
    
    var intValue: Int {
        switch self {
        case .mint: return 0
        case .good: return 1
        case .fair: return 2
        case .poor: return 3
        }
    }
    
}
