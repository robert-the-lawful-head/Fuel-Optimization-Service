import { AircraftSizes } from "../enums/aircraft-sizes";
import { FboFavoriteAircraft } from "./favorites/favoriteAircraft";

export interface CustomerAircraftsViewModel {
    oid: number;
    groupId: number | null;
    customerId: number;
    company: string;
    aircraftId: number;
    tailNumber: string;
    size: AircraftSizes | null;
    basedPaglocation: string;
    networkCode: string;
    addedFrom: number | null;
    pricingTemplateId: number | null;
    pricingTemplateName: string;
    make: string;
    model: string;
    fuelCapacityGal: number | null;
    isFuelerlinxNetwork: boolean;
    isCompanyPricing: boolean;
    iCAOAircraftCode: string;
    phone: string;
    customerInfoByGroupId: number;
    fuelerlinxCompanyId: number | null;
    favoriteAircraft: FboFavoriteAircraft | null;
    aircraftSizeDescription: string;
    notes: CustomerAircraftNoteDto[];
}
export interface CustomerAircraftNoteDto {
    oid: number;
    customerAircraftId: number;
    fboId: number | null;
    notes: string;
    lastUpdatedUtc: string | null;
    lastUpdatedByUserId: number | null;
}
