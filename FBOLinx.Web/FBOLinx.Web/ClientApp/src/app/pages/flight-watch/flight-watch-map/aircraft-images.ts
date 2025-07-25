export const AircraftIconSize = 22;
export const AircraftIconScale = 1.5;

export interface AircraftImageData {
    blueReverseUrl: string,
    blueUrl: string,
    fuelerlinxUrl: string,
    fuelerlinxReverseUrl: string,
    description: string,
    fillColor: string,
    id: string,
    label: string,
    reverseUrl: string,
    size: number,
    url: string,
}
export const AIRCRAFT_IMAGES: AircraftImageData[]= [
    {
        blueReverseUrl: '/assets/img/map-markers/default-release-reverse.svg',
        blueUrl: '/assets/img/map-markers/default-release.svg',
        fuelerlinxUrl: '/assets/img/map-markers/default-FUELERLINX.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/default-FUELERLINX-reverse.svg',
        description: 'Unspecified Powered Aircraft',
        fillColor: '#3daf2c',
        id: 'A0',
        label: 'Unspecified',
        reverseUrl: '/assets/img/map-markers/default-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/default.svg',
    },
    {
        blueReverseUrl: '/assets/img/map-markers/A1-release-reverse.svg',
        blueUrl: '/assets/img/map-markers/A1-release.svg',
        fuelerlinxUrl: '/assets/img/map-markers/A1-FUELERLINX.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/A1-FUELERLINX-reverse.svg',
        description: 'Less than 15.5k lbs',
        fillColor: '#737373',
        id: 'A1',
        label: 'Light Jet',
        reverseUrl: '/assets/img/map-markers/A1-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/A1.svg',
    },
    {
        blueReverseUrl: '/assets/img/map-markers/A2-release-reverse.svg',
        blueUrl: '/assets/img/map-markers/A2-release.svg',
        fuelerlinxUrl: '/assets/img/map-markers/A2-FUELERLINX.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/A2-FUELERLINX-reverse.svg',
        description: '15.k to 75k lbs',
        fillColor: '#dabe0e',
        id: 'A2',
        label: 'Small Jet',
        reverseUrl: '/assets/img/map-markers/A2-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/A2.svg',
    },
    {
        blueReverseUrl: '/assets/img/map-markers/A3-release-reverse.svg',
        blueUrl: '/assets/img/map-markers/A3-release.svg',
        fuelerlinxUrl: '/assets/img/map-markers/A3-FUELERLINX.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/A3-FUELERLINX-reverse.svg',
        description: '75k to 300k lbs',
        fillColor: '#f7e600',
        id: 'A3',
        label: 'Large Jet',
        reverseUrl: '/assets/img/map-markers/A3-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/A3.svg',
    },
    {
        blueReverseUrl: '/assets/img/map-markers/A4-release-reverse.svg',
        blueUrl: '/assets/img/map-markers/A4-release.svg',
        fuelerlinxUrl: '/assets/img/map-markers/A4-FUELERLINX.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/A4-FUELERLINX-reverse.svg',
        description: 'High Vortex, Large',
        fillColor: '#ad36f2',
        id: 'A4',
        label: 'HVL Jet',
        reverseUrl: '/assets/img/map-markers/A4-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/A4.svg',
    },
    {
        blueReverseUrl: '/assets/img/map-markers/A5-release-reverse.svg',
        blueUrl: '/assets/img/map-markers/A5-release.svg',
        fuelerlinxUrl: '/assets/img/map-markers/A5-FUELERLINX.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/A5-FUELERLINX-reverse.svg',
        description: 'Greater than 300k lbs',
        fillColor: '#ed4077',
        id: 'A5',
        label: 'Heavy Jet',
        reverseUrl: '/assets/img/map-markers/A5-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/A5.svg',
    },
    {
        blueReverseUrl: '/assets/img/map-markers/A7-release-reverse.svg',
        blueUrl: '/assets/img/map-markers/A7-release.svg',
        fuelerlinxUrl: '/assets/img/map-markers/A7-FUELERLINX.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/A7-FUELERLINX-reverse.svg',
        description: '',
        fillColor: '#ffffff',
        id: 'A7',
        label: 'Helicopter',
        reverseUrl: '/assets/img/map-markers/A7-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/A7.svg',
    },
    {
        blueReverseUrl: '/assets/img/map-markers/default-release-reverse.svg',
        blueUrl: '/assets/img/map-markers/default-release.svg',
        fuelerlinxUrl: '/assets/img/map-markers/default-FUELERLINX.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/default-FUELERLINX-reverse.svg',
        description: '',
        fillColor: '#2b62ae',
        id: 'default',
        label: 'Other',
        reverseUrl: '/assets/img/map-markers/default-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/default.svg',
    },
    {
        blueReverseUrl: '/assets/img/map-markers/airplane-CLIENT-reverse.svg',
        blueUrl: '/assets/img/map-markers/airplane-CLIENT.svg',
        fuelerlinxUrl: '/assets/img/map-markers/airplane-CLIENT.svg',
        fuelerlinxReverseUrl: '/assets/img/map-markers/airplane-CLIENT-reverse.svg',
        description: '',
        fillColor: '#2b62ae',
        id: 'client',
        label: 'Other',
        reverseUrl: '/assets/img/map-markers/airplane-CLIENT-reverse.svg',
        size: 65,
        url: '/assets/img/map-markers/airplane-CLIENT.svg',
    }
];
