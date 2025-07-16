//
//  NewReviewView.swift
//  BookShop
//
//  Created by dennis savchenko on 10/06/2025.
//

import SwiftUI

struct NewReviewView: View {
    
    @State private var model = ReviewViewModel()
    @Environment(\.dismiss) private var dismiss
    
    let customerId: Int
    let itemId: Int
    
    @State var rating: Int = 0
    @State var text: String = ""
    @State var submitTry = false
    @State var addedSuccessfully: Bool? = nil
    @State var submitDisabled = false
    private let maxLength = 500
    
    
    var body: some View {
        VStack(alignment: .leading, spacing: 16) {
            Text("RATING")
                .foregroundStyle(.gray)
            HStack(spacing: 0) {
                Image(systemName: rating >= 1 ? "star.fill" : "star")
                    .onTapGesture {
                        rating = 1
                    }
                Image(systemName: rating >= 2 ? "star.fill" : "star")
                    .onTapGesture {
                        rating = 2
                    }
                Image(systemName: rating >= 3 ? "star.fill" : "star")
                    .onTapGesture {
                        rating = 3
                    }
                Image(systemName: rating >= 4 ? "star.fill" : "star")
                    .onTapGesture {
                        rating = 4
                    }
                Image(systemName: rating >= 5 ? "star.fill" : "star")
                    .onTapGesture {
                        rating = 5
                    }
                Spacer()
                Text("\(rating)/5")
                    .fontWeight(.medium)
            }
            .foregroundStyle(.star)
            .font(.largeTitle)
            if submitTry && rating == 0 {
                errorMessage(message: "Rate the item on scale from 1 to 5!")
            }
            Text("REVIEW")
                .foregroundStyle(.gray)
            TextEditor(text: $text)
                .padding()
                .background {
                    RoundedRectangle(cornerRadius: 16)
                        .stroke(.gray, lineWidth: 1)
                }
                .frame(height: 200)
                .onChange(of: text) { oldValue, newValue in
                    if newValue.count > maxLength {
                        text = String(newValue.prefix(maxLength))
                    }
                }
            if submitTry && text.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty {
                errorMessage(message: "Review cannot be empty!")
            }
            Spacer()
            Button {
                submitTry = true
                if rating > 0 && !text.trimmingCharacters(in: .whitespacesAndNewlines).isEmpty {
                    Task {
                        let review = SendReview(itemId: itemId, customerId: customerId, text: text, rating: rating)
                        addedSuccessfully = await model.submitReview(review)
                        submitDisabled = true
                        DispatchQueue.main.asyncAfter(deadline: .now() + 2) {
                                if addedSuccessfully == true {
                                    dismiss()
                                } else {
                                    addedSuccessfully = nil
                                    submitDisabled = false
                                }
                            }
                    }
                }
            } label: {
                HStack(spacing: 4) {
                    Image(systemName: "paperplane.fill")
                        .fontWeight(.medium)
                    Text("Submit")
                        .fontWeight(.bold)
                }
                .foregroundStyle(.white)
                .padding()
                .frame(maxWidth: .infinity)
                .background {
                    RoundedRectangle(cornerRadius: 12)
                }
            }.disabled(submitDisabled)
        }
        .navigationTitle("New Review")
        .overlay {
            if addedSuccessfully != nil {
                if addedSuccessfully ?? false {
                    infoMessage(message: "Your review was successfully added!")
                } else {
                    infoMessage(message: "Something went wrong. Try again!")
                }
            }
        }
        .padding()
    }
    
    func infoMessage(message: String) -> some View {
        Text(message)
            .foregroundStyle(.gray)
            .fontWeight(.semibold)
            .padding()
            .background {
                RoundedRectangle(cornerRadius: 16)
                    .fill(.thinMaterial)
            }
    }
    
    func errorMessage(message: String) -> some View {
        Text(message)
            .foregroundStyle(.errorMessage)
            .font(.footnote)
            .fontWeight(.medium)
            .padding(.top, -8)
    }
    
}

#Preview {
    NewReviewView(customerId: 1, itemId: 1)
}
