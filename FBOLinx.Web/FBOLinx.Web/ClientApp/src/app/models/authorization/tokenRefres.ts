export interface ExchangeRefreshTokenResponse {
    authToken: string;
    refreshToken: string;
    authTokenExpiration: Date;
    refreshTokenExpiration: Date;
}