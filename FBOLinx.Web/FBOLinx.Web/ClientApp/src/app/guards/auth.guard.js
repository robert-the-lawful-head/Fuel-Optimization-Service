"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var AuthGuard = /** @class */ (function () {
    function AuthGuard(router, authenticationService) {
        this.router = router;
        this.authenticationService = authenticationService;
    }
    AuthGuard.prototype.canActivate = function (route, state) {
        var currentUser = this.authenticationService.currentUserValue;
        if (currentUser) {
            // logged in so return true
            return true;
        }
        // not logged in so redirect to login page with the return url
        this.router.navigate(["/landing-site"], {
            queryParams: { returnUrl: state.url },
        });
        return false;
    };
    return AuthGuard;
})();
exports.AuthGuard = AuthGuard;
//# sourceMappingURL=auth.guard.js.map
