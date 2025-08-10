//
//  ContentView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 13/06/2025.
//

import SwiftUI

struct ContentView: View {
    
    @State var accessToken: String? = nil
    @State var refreshToken: String? = nil
    @State var employeeId: Int? = nil
    
    @State private var logInViewModel = LogInViewModel()
    
    @State var loggedIn = false
    @State var needsToLogIn = false
    
    var body: some View {
        if loggedIn {
            WorkspaceView()
        } else if needsToLogIn {
            LogInView(loggedIn: $loggedIn)
        } else {
            ProgressView()
                .padding(100)
                .onAppear {
                    accessToken = KeychainHelper.load(forKey: "access_token")
                    refreshToken = KeychainHelper.load(forKey: "refresh_token")
                    if let accessToken = accessToken, let refreshToken = refreshToken {
                        Task {
                            await logInViewModel.refresh(refreshCredentials: UserTokens(id: 0, accessToken: accessToken, refreshToken: refreshToken))
                            if logInViewModel.statusCode == 200 {
                                loggedIn = true
                            } else {
                                needsToLogIn = true
                            }
                        }
                    }
                    if let employeeIdString = KeychainHelper.load(forKey: "user_id") {
                        employeeId = Int(employeeIdString)
                    }
                    print(accessToken ?? "")
                    print(refreshToken ?? "")
                    print(employeeId ?? 0)
                }
        }
    }
}

#Preview {
    ContentView()
}
