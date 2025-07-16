//
//  OrderView.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import SwiftUI

struct OrderView: View {
    
    @State private var model = OrderViewModel()
    let orderId: Int
    
    var body: some View {
        ScrollView {
            VStack(alignment: .leading, spacing: 20) {
                if model.isLoading {
                    ProgressView("Loading...")
                } else if let order = model.order {
                    header(order: order)
                    statusGraph(order: order)
                    Text("Ordered Items")
                        .font(.title3)
                        .bold()
                    ForEach(order.items, id: \.item.id) { orderItem in
                        Divider()
                        NavigationLink {
                            ItemView(itemId: orderItem.item.id, customerId: order.customerId)
                        } label: {
                            ItemListView(orderItem: orderItem, isLoaded: false)
                        }
                        .buttonStyle(.plain)
                        .background(.white)
                    }
                }
            }
            .padding()
        }
        .navigationTitle("Order Details")
        .kerning(-0.3)
        .refreshable {
            await model.loadOrder(orderId: orderId)
        }
        .task {
            await model.loadOrder(orderId: orderId)
        }
    }
    
    func header(order: Order) -> some View {
        HStack(alignment: .top) {
            VStack(alignment: .leading) {
                Text("Order ID: \(order.id)")
                    .font(.title2)
                    .fontWeight(.semibold)
                Text(Date.dateFormatter().string(from: order.confirmedAt))
                    .font(.footnote)
                    .foregroundStyle(.gray)
            }
            Spacer()
            VStack(alignment: .trailing, spacing: 8) {
                Text("Total:")
                    .font(.footnote)
                    .fontWeight(.semibold)
                Text("\(String(format: "%.2f", order.totalPrice))$")
                    .font(.title2)
                    .fontWeight(.bold)
            }
        }
    }
    
    func statusGraph(order: Order) -> some View {
        VStack(alignment: .leading) {
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
        VStack(alignment: .leading) {
            HStack(spacing: 12) {
                StatusBadge(status: status, isActive: isActive)
                Text("\(timeStamp != nil ? Date.timeStampFormatter().string(from: timeStamp ?? Date.now) : "")")
                    .font(.footnote)
                    .foregroundStyle(.gray)
            }
            if status != OrderStatus.delivered && status != OrderStatus.cancelled {
                RoundedRectangle(cornerRadius: 16)
                    .frame(width: 2, height: 20)
                    .padding(.leading)
                    .padding(4)
                    .foregroundStyle(isActive ? Color.getBdColor(for: status) : .gray)
            }
        }
    }
    
}

#Preview {
    OrderView(orderId: 3)
}
