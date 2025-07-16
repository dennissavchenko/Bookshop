//
//  OrderListView.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import SwiftUI

struct OrderListView: View {
    
    var order: SimpleOrder
    
    var body: some View {
        HStack(alignment: .top) {
            VStack(alignment: .leading) {
                Text("Order ID: \(order.id)")
                    .font(.title3)
                    .fontWeight(.semibold)
                Text(Date.dateFormatter().string(from: order.lastUpdatedAt))
                    .font(.footnote)
                    .foregroundStyle(.gray)
            }
            Spacer()
            VStack(alignment: .trailing) {
                Text("\(String(format: "%.2f", order.totalPrice))$")
                    .font(.title2)
                    .fontWeight(.bold)
                StatusBadge(status: order.status, isActive: true)
            }
        }
        .padding()
        .background(
            RoundedRectangle(cornerRadius: 16)
                .fill(.white)
        )
        .overlay(
            RoundedRectangle(cornerRadius: 16)
                .stroke(.gray.opacity(0.5), lineWidth: 1)
        )
        .kerning(-0.3)
    }
    
}

#Preview {
    OrderListView(order: SimpleOrder(id: 1, status: OrderStatus.confirmed, lastUpdatedAt: Date.now, totalPrice: 12.3, customerId: 1))
}
