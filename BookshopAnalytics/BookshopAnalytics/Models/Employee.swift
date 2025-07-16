//
//  Employee.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 25/06/2025.
//

enum Experience: String, CaseIterable {
    case junior = "Junior"
    case middle = "Middle"
    case senior = "Senior"
    case unknown = "Unknown"
}

struct Employee: Identifiable, Decodable {
    var id: Int
    var name: String
    var surname: String
    var email: String
    var username: String
    var password: String?
    var salary: Double
    private var experience: Int
    var experienceType: Experience {
        experience == 0 ? .junior : experience == 1 ? .middle : experience == 2 ? .senior : .unknown
    }
}
