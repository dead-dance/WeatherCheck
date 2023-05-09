export interface IDistrict {
    id: number;
    division_id: number;
    name: string;
    bn_name: string;
    lat: number;
    long: number;
}


export class District implements IDistrict {
    id: number = 0;
    division_id: number;
    name: string;
    bn_name: string;
    lat: number;
    long: number;
}
