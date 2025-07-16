//
//  StatusBadge.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import SwiftUI

struct StatusBadge: View {
    
    var status: OrderStatus
    var isActive: Bool
    
    var body: some View {
        Text(status.rawValue)
            .font(.footnote)
            .fontWeight(.bold)
            .foregroundStyle(isActive ? Color.getBdColor(for: status) : .gray)
            .padding(8)
            .background(
                RoundedRectangle(cornerRadius: 12)
                    .fill(isActive ? Color.getBgColor(for: status) : Color(.systemGray6))
            )
            .overlay(
                RoundedRectangle(cornerRadius: 12)
                    .stroke(isActive ? Color.getBdColor(for: status) : .gray, lineWidth: 2)
            )
    }
}

#Preview {
    StatusBadge(status: OrderStatus.confirmed, isActive: false)
}
