import { IndexRouteProps, LayoutRouteProps, PathRouteProps } from "react-router-dom";
import { Counter } from "./pages/Counter";
import { FetchData } from "./pages/FetchData";
import { Home } from "./pages/Home";
import { Listings } from "./pages/Listings";
import { RealEstates } from "./pages/services/Listings/RealEstates";

/**
 * The endpoint routes of the application.
 */
const AppRoutes: Array<PathRouteProps | LayoutRouteProps | IndexRouteProps> = [
	{
		index: true,
		element: <Home />
	},
	{
		path: '/counter',
		element: <Counter />
	},
	{
		path: '/fetch-data',
		element: <FetchData />
	},
	{
		path: "/real-estates",
		element: <RealEstates />
	},
	{
		path: "/listings/vehicles",
		element: <Listings />
	},
	{
		path: "/listings/businesses",
		element: <Listings />
	},
	{
		path: "/listings/home-and-kitchen",
		element: <Listings />
	},
	{
		path: "/listings/personal-stuffs",
		element: <Listings />
	},
	{
		path: "/listings/electronics",
		element: <Listings />
	},
	{
		path: "/listings/industry",
		element: <Listings />
	},
	{
		path: "/listings/jobs",
		element: <Listings />
	},
	{
		path: "/listings/services",
		element: <Listings />
	},
];

export default AppRoutes;