//
//  OrderDetailsView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 10/08/2025.
//

import SwiftUI

struct OrderDetailsView: View {
    
    @State private var model = OrdersViewModel()
    var orderId: Int
    
    var body: some View {
        ScrollView {
            VStack(alignment: .leading, spacing: 20) {
                if let order = model.selectedOrder {
                    header(order: order)
                    statusGraph(order: order)
                    Text("Ordered Items")
                        .font(.title3)
                        .bold()
                    ForEach(order.items, id: \.item.id) { orderItem in
                        Divider()
                        ItemListView(orderItem: orderItem, isLoaded: false)
                        .buttonStyle(.plain)
                    }
                }
            }
            .padding()
        }
        .navigationTitle("Order Details")
        .kerning(-0.3)
        .task {
            await model.fetchOrder(orderId)
        }
        .onChange(of: orderId) {
            Task {
                await model.fetchOrder(orderId)
            }
        }
        .frame(width: 280)
    }
    
    func header(order: Order) -> some View {
        HStack(alignment: .top) {
            VStack(alignment: .leading) {
                Text("Order ID: \(order.id)")
                    .font(.title2)
                    .fontWeight(.semibold)
                Text(order.confirmedAt.customFormatDateFormatter(format: "E. MMM d, yyyy"))
                    .font(.footnote)
                    .foregroundStyle(.gray)
            }
            Spacer()
            VStack(alignment: .trailing, spacing: 8) {
                Text("Total:")
                    .font(.footnote)
                    .fontWeight(.semibold)
                Text("$\(String(format: "%.2f", order.totalPrice))")
                    .font(.title2)
                    .fontWeight(.bold)
            }
        }
    }
    
    func statusGraph(order: Order) -> some View {
        VStack(alignment: .leading, spacing: 0) {
            statusGraphItem(status: OrderStatus.confirmed, timeStamp: order.confirmedAt, isActive: true)
            if order.cancelledAt != nil {
                statusGraphItem(status: OrderStatus.cancelled, timeStamp: order.cancelledAt, isActive: true)
            } else {
                statusGraphItem(status: OrderStatus.preparation, timeStamp: order.preparationStartedAt, isActive: order.preparationStartedAt != nil)
                statusGraphItem(status: OrderStatus.shipped, timeStamp: order.shippedAt, isActive: order.shippedAt != nil)
                statusGraphItem(status: OrderStatus.delivered, timeStamp: order.deliveredAt, isActive: order.deliveredAt != nil)
            }
        }
    }
    
    func statusGraphItem(status: OrderStatus, timeStamp: Date?, isActive: Bool) -> some View {
        VStack(alignment: .leading, spacing: 0) {
            HStack(spacing: 12) {
                StatusBadge(status: status, isActive: isActive)
                Text("\(timeStamp != nil ? (timeStamp ?? Date.now).customFormatDateFormatter(format: "dd.MM.yyyy | HH:mm") : "")")
                    .font(.footnote)
                    .foregroundStyle(.gray)
            }
            if status != OrderStatus.delivered && status != OrderStatus.cancelled {
                RoundedRectangle(cornerRadius: 16)
                    .frame(width: 2, height: 20)
                    .padding(.leading)
                    .foregroundStyle(isActive ? Color.accentColor : .gray)
            }
        }
    }
    
}

#Preview {
    OrderDetailsView(orderId: 2)
}
