//
//  EmployeeView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 25/06/2025.
//

import SwiftUI

struct EmployeeView: View {
    
    var employeeId: Int
    
    @State var model = EmployeeViewModel()
    
    var body: some View {
        VStack {
            Text("Employee \(employeeId)")
            Text("Employee's name: \(model.employee?.experienceType.rawValue ?? "Unknown")")
                .onAppear {
                    Task {
                        await model.fetchEmployee(employeeId: employeeId)
                    }
                }
        }
    }
}

#Preview {
    EmployeeView(employeeId: 3)
}
