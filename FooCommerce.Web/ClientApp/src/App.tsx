import React, { Component } from 'react';
import { Route, Routes } from 'react-router-dom';
import AppRoutes from './AppRoutes';
import './styles/app.scss';
export default class App extends Component {
	static displayName = App.name;

	render() {
		return (
			// <Layout>
			<Routes>
				{AppRoutes.map((route, index) => {
					const { element, ...props } = route;
					return <Route key={index} {...props} element={element} />;
				})}
			</Routes>
			// </Layout>
		);
	}
}
