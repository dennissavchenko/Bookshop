//
//  WorkspaceSidebarView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 26/06/2025.
//

import SwiftUI

struct WorkspaceSidebarView: View {
    
    static let itemTabs = [SidebarTab(title: "All", icon: "book.closed")]
    
    static let orderTabs = [SidebarTab(title: "Confirmed", icon: "checkmark.circle"), SidebarTab(title: "Preparation", icon: "clock.arrow.2.circlepath"), SidebarTab(title: "Shipped", icon: "shippingbox")]
    
    @Binding var sidebarSelection: SidebarTab?
    
    var body: some View {
        List(selection: $sidebarSelection) {
            Section("Items") {
                ForEach(WorkspaceSidebarView.itemTabs) { sidebarTab in
                    Label(sidebarTab.title, systemImage: sidebarTab.icon)
                        .tag(sidebarTab)
                }
            }
            Section("Orders") {
                ForEach(WorkspaceSidebarView.orderTabs) { sidebarTab in
                    Label(sidebarTab.title, systemImage: sidebarTab.icon)
                        .tag(sidebarTab)
                }
            }
        }
    }
}

#Preview {
    WorkspaceSidebarView(sidebarSelection: .constant(nil))
}
