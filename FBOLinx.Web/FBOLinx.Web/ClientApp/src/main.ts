import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { environment } from './environments/environment';

export const getBaseUrl = () => document.getElementsByTagName('base')[0].href;

const providers = [{ deps: [], provide: 'BASE_URL', useFactory: getBaseUrl }];

if (environment.production) {
    enableProdMode();
}

platformBrowserDynamic(providers)
    .bootstrapModule(AppModule)
    .catch((err) => console.error(err));
