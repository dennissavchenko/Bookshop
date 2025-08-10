
import SwiftUI

struct StatusBadge: View {
    
    var status: OrderStatus
    var isActive: Bool
    
    var body: some View {
        Text(status.rawValue)
            .font(.footnote)
            .fontWeight(.medium)
            .foregroundStyle(isActive ? .white : .gray)
            .padding(8)
            .background(
                RoundedRectangle(cornerRadius: 8)
                    .fill(isActive ? Color.accentColor : Color(.gray.withAlphaComponent(0.15)))
            )
    }
}

#Preview {
    StatusBadge(status: OrderStatus.confirmed, isActive: true)
}
