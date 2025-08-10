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
    @State var selectedOrderId: Int? = nil
    @State var selectedTab: SidebarTab? = nil
    @State var isAlertPresented: Bool = false
    @State var itemToBeDeletedId: Int? = nil
    
    @State var addNewItem = false
    
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
                            }
                        }
                    } else {
                        List {
                            ForEach(ordersViewModel.orders) { order in
                                Text("\(order.id)")
                                    .background(.white)
                                    .onTapGesture {
                                        selectedOrderId = order.id
                                    }
                            }
                        }
                        .onChange(of: selectedTab) { oldValue, newValue in
                            if newValue?.title != "All" {
                                Task {
                                    await ordersViewModel.fetchOrders(orderStatus: selectedTab?.title == "Confirmed" ? .confirmed : selectedTab?.title == "Preparation" ? .preparation : .shipped)
                                }
                            }
                        }
                    }
                    if selectedItem != nil {
                        if let item = itemsViewModel.selectedItem {
                            ItemDetailsView(item: item)
                        }
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
                                        await itemsViewModel.deleteItem(itemId)
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
                Button("", systemImage: "plus") {
                    addNewItem.toggle()
                }
            }
        }
        .sheet(isPresented: $addNewItem) {
            AddNewItem()
        }
    }
}

#Preview {
    WorkspaceView()
}
