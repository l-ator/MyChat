export interface IOpResult {
	succeeded: boolean
	errors: string[]
	content:any
}

export interface IAuthOpResult extends IOpResult {
	id:string
	username:string
	email: string 
}