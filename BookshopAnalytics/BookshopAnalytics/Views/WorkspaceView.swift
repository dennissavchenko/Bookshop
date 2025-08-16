//
//  WorkSpace.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 25/06/2025.
//

import SwiftUI

struct WorkspaceView: View {
    
    @State var itemsViewModel = ItemsViewModel()
    @State var ordersViewModel = OrdersViewModel()
    
    @State var selectedItem: SimpleItem? = nil
    @State var selectedOrder: SimpleOrder? = nil
    @State var selectedTab: SidebarTab? = nil
    @State var isAlertPresented: Bool = false
    @State var itemToBeDeletedId: Int? = nil
    
    @State var addNewItem = false
    
    func getOrderListView(order: SimpleOrder) -> some View {
        HStack {
            VStack {
                Text("Order ID: \(order.id)")
                Text(order.lastUpdatedAt.customFormatDateFormatter(format: "MMM, d yyyy"))
                    .font(.footnote)
            }
            Spacer()
            if order.status == .confirmed || order.status == .preparation {
                Button(order.status == .confirmed ? "Start Preparation" : "Ship") {
                    Task {
                        await ordersViewModel.changeStatus(order.id, order.status.intValue + 1)
                        await ordersViewModel.fetchOrders(orderStatus: order.status)
                    }
                }
            }
        }
    }
    
    var body: some View {
        NavigationSplitView {
            WorkspaceSidebarView(sidebarSelection: $selectedTab)
        } detail: {
            ZStack {
                HStack(spacing: 0) {
                    if selectedTab?.title == "All" {
                        List(selection: $selectedItem) {
                            ForEach(itemsViewModel.items) { item in
                                HStack {
                                    Text(item.name)
                                    Spacer()
                                    Button("Edit") {
                                        Task {
                                            await itemsViewModel.getAddItem(item.id)
                                            addNewItem.toggle()
                                        }
                                    }
                                    Button("Delete") {
                                        itemToBeDeletedId = item.id
                                        isAlertPresented = true
                                    }
                                }
                                .tag(item)
                            }
                        }
                        .onAppear {
                            Task {
                                await itemsViewModel.fetchItems()
                                withAnimation {
                                    selectedOrder = nil
                                }
                            }
                        }
                    } else if selectedTab == nil {
                        Text("Welcome to your workspace!")
                    } else {
                        List(selection: $selectedOrder) {
                            ForEach(ordersViewModel.orders) { order in
                                getOrderListView(order: order).tag(order)
                            }
                        }
                        .onAppear {
                            withAnimation {
                                selectedItem = nil
                            }
                            Task {
                                await ordersViewModel.fetchOrders(orderStatus: selectedTab?.title == "Confirmed" ? .confirmed : selectedTab?.title == "Preparation" ? .preparation : .shipped)
                            }
                        }
                        .onChange(of: selectedTab) {
                            if selectedTab?.title != "All" {
                                withAnimation {
                                    selectedItem = nil
                                }
                                Task {
                                    await ordersViewModel.fetchOrders(orderStatus: selectedTab?.title == "Confirmed" ? .confirmed : selectedTab?.title == "Preparation" ? .preparation : .shipped)
                                }
                            } else {
                                withAnimation {
                                    selectedOrder = nil
                                }
                            }
                        }
                    }
                    if selectedItem != nil {
                        if let item = itemsViewModel.selectedItem {
                            ItemDetailsView(item: item)
                        }
                    } else if selectedOrder != nil {
                        OrderDetailsView(orderId: selectedOrder?.id ?? 0)
                    }
                }
                .onChange(of: selectedItem) {
                    Task {
                        if let id = selectedItem?.id {
                            await itemsViewModel.fetchItem(id)
                        }
                    }
                }
                if isAlertPresented {
                    Color.black.opacity(0.2)
                        .edgesIgnoringSafeArea(.all)
                        .onTapGesture {
                            isAlertPresented = false
                        }
                    
                    VStack(spacing: 20) {
                        Text("Are you sure you want to delete this item?")
                            .font(.headline)
                        HStack(spacing: 16) {
                            Button("Cancel") {
                                isAlertPresented = false
                            }
                            .buttonStyle(.borderedProminent)
                            Button("Yes") {
                                Task {
                                    if let itemId = itemToBeDeletedId {
                                        if let selection = selectedItem {
                                            if selection.id == itemId {
                                                withAnimation {
                                                    selectedItem = nil
                                                }
                                            }
                                        }
                                        _ = await itemsViewModel.deleteItem(itemId)
                                        await itemsViewModel.fetchItems()
                                    }
                                }
                                isAlertPresented = false
                            }
                        }
                    }
                    .padding()
                    .background(Color.white)
                    .cornerRadius(12)
                    .shadow(radius: 10)
                    .padding(40)
                }
            }
        }
        .toolbar {
            ToolbarItem() {
                if selectedTab?.title == "All" {
                    Button("", systemImage: "plus") {
                        itemsViewModel.selectedAddItem = nil
                        addNewItem.toggle()
                    }
                }
            }
        }
        .navigationTitle("Workspace")
        .sheet(isPresented: $addNewItem) {
            if let itemToUpdate = itemsViewModel.selectedAddItem {
                AddNewItem(item: itemToUpdate)
                    .onDisappear {
                        Task {
                            if let id = selectedItem?.id {
                                await itemsViewModel.fetchItem(id)
                            }
                            await itemsViewModel.fetchItems()
                        }
                    }
            } else {
                AddNewItem()
                    .onDisappear {
                        Task {
                            if let id = selectedItem?.id {
                                await itemsViewModel.fetchItem(id)
                            }
                            await itemsViewModel.fetchItems()
                        }
                    }
            }
        }
    }
}

#Preview {
    WorkspaceView()
}
