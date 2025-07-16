//
//  CustomerOrdersView.swift
//  BookShop
//
//  Created by dennis savchenko on 09/06/2025.
//

import SwiftUI

struct CustomerOrdersView: View {
    
    @State private var model = CustomerOrdersViewModel()
    let customerId = 1
    
    var body: some View {
        NavigationStack {
            ScrollView {
                VStack(spacing: 16) {
                    ForEach(model.orders) { order in
                        NavigationLink {
                            OrderView(orderId: order.id)
                        } label: {
                            OrderListView(order: order)
                        }
                        .buttonStyle(.plain)
                    }
                }
                .padding()
            }
            .navigationTitle("Your Orders")
            .refreshable {
                Task {
                    await model.fetchOrders(for: customerId)
                }
            }
            .task {
                await model.fetchOrders(for: customerId)
            }
        }
    }
}

#Preview {
    CustomerOrdersView()
}
