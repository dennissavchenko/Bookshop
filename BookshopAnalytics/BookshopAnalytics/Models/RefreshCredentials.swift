struct RefreshCredentials: Encodable {
    var id: Int
    var accessToken: String
    var refreshToken: String
}
