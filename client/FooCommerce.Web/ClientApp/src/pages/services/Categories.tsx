import { IconName } from '@fortawesome/fontawesome-svg-core';
import { LinkProps } from 'react-router-dom';

interface IListingCategory {
	/**
	 * The title of the category.
	 */
	title: string;
	/**
	 * The icon of the category.
	 * @see https://fontawesome.com/icons
	 * @example 'home'
	 */
	icon: IconName;
}

const ListingCategories: Array<LinkProps & IListingCategory> = [
	{
		to: '/real-estates',
		icon: "home",
		title: "Real Estates",
	},
	{
		to: "/listings/vehicles",
		icon: "car",
		title: "Vehicles",
	},
	{
		to: "/listings/businesses",
		icon: "store",
		title: "Businesses",
	},
	{
		to: "/listings/home-and-kitchen",
		icon: "couch",
		title: "Home & Kitchen",
	},
	{
		to: "/listings/personal-stuffs",
		icon: "hand",
		title: "Personal Stuffs",
	},
	{
		to: "/listings/electronics",
		icon: "television",
		title: "Electronics",
	},
	{
		to: "/listings/industry",
		icon: "wrench",
		title: "Industry",
	},
	{
		to: "/listings/jobs",
		icon: "briefcase",
		title: "Jobs",
	},
	{
		to: "/listings/services",
		icon: "person-dress",
		title: "Services",
	},
];

export default ListingCategories;