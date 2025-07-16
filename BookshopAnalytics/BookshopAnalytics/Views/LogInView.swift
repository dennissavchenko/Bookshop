//
//  LogInView.swift
//  BookShopAnalytics
//
//  Created by dennis savchenko on 25/06/2025.
//

import SwiftUI

struct LogInView: View {
    
    @State private var model = LogInViewModel()
    
    @State var usernameOrEmail: String = ""
    @State var password: String = ""
    
    @State var usernameOrEmailError: Bool = false
    @State var passwordError: Bool = false
    
    @State var showEmployeeInfo: Bool = false
    
    @Environment(\.openWindow) private var openWindow
    @Environment(\.dismiss) private var dismiss
    
    func isEmpty(_ text: String) -> Bool {
        text.trimmingCharacters(in: .whitespaces).isEmpty
    }
    
    var body: some View {
        NavigationStack {
            VStack {
                Image(systemName: "book.fill")
                    .font(.title)
                Text("Book Shop")
                    .italic()
                    .bold()
                VStack(alignment: .leading) {
                    TextField("Username or email", text: $usernameOrEmail)
                    if usernameOrEmailError {
                        getErrorMessage(message: model.statusCode == 404 ? "User was not found!" : "This field cannot be empty!")
                    }
                }
                VStack(alignment: .leading) {
                    TextField("Password", text: $password)
                    if passwordError {
                        getErrorMessage(message: model.statusCode == 401 ? "Incorrect password. Access denied!" : "This field cannot be empty!")
                    }
                }
                Button("Log In") {
                    if usernameOrEmail.isEmptyW {
                        usernameOrEmailError = true
                    }
                    if password.isEmptyW {
                        passwordError = true
                    }
                    if !usernameOrEmail.isEmptyW && !password.isEmptyW {
                        Task {
                            await model.logIn(logInCredentials: LogInCredentials(usernameOrEmail: usernameOrEmail, password: password))
                            if model.statusCode == 404 {
                                usernameOrEmailError = true
                            }
                            else if model.statusCode == 401 {
                                passwordError = true
                            } else if model.statusCode == 200 {
                                if let id = model.userTokens?.id {
                                    openWindow(id: "employee-info", value: id)
                                    dismiss()
                                }
                            }
                        }
                    }
                }
                if model.userTokens != nil {
                    Text(model.userTokens?.accessToken ?? "")
                    Text(model.userTokens?.refreshToken ?? "")
                }
            }
            .padding()
            .navigationTitle("Book Shop Analytics")
            .onChange(of: usernameOrEmail) { oldValue, newValue in
                if oldValue.isEmptyW && !newValue.isEmptyW {
                    usernameOrEmailError = false
                }
            }
            .onChange(of: password) { oldValue, newValue in
                if oldValue.isEmptyW && !newValue.isEmptyW {
                    passwordError = false
                }
            }
        }
    }
    
    func getErrorMessage(message: String) -> some View {
        HStack(spacing: 4) {
            Image(systemName: "exclamationmark.circle.fill")
            Text(message)
        }
        .font(.footnote)
        .foregroundStyle(.red)
    }
    
}

#Preview {
    LogInView()
}
