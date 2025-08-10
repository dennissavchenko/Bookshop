import SwiftUI

struct CompletionBadge: View {
    
    var isSuccessful: Bool
    
    var body: some View {
        VStack(spacing: 8) {
            Image(systemName: isSuccessful ? "checkmark.circle" : "xmark.circle")
                .font(.system(size: 48))
            Text(isSuccessful ? "Success!" : "Error Occured!")
                .font(.title2)
        }
        .padding()
        .background {
            RoundedRectangle(cornerRadius: 16)
                .fill(.thinMaterial)
        }
    }
}

#Preview {
    CompletionBadge(isSuccessful: false)
}
