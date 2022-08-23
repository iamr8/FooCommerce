import React, { ReactElement } from 'react';
import { createRoot } from 'react-dom/client';
import { BrowserRouter } from 'react-router-dom';
import App from './App';
import * as serviceWorkerRegistration from './serviceWorkerRegistration';
import reportWebVitals from './reportWebVitals';
import { Helmet, HelmetProvider } from 'react-helmet-async';

const baseUrl = document.getElementsByTagName('base')[0].getAttribute('href') as string;
const root = createRoot(document.getElementById('root') as HTMLElement);

var fonts: ReactElement;
if (process.env.NODE_ENV === 'development') {
	require("./styles/base/_fonts.scss");
	fonts = (<></>);
} else {
	fonts = (<>
		<link rel="preconnect" href="https://fonts.googleapis.com" />
		<link rel="preconnect" href="https://fonts.gstatic.com" crossOrigin='anonymous' />
		<link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;600;800&display=swap" rel="stylesheet" />
	</>);
}

root.render(

	<HelmetProvider>
		<BrowserRouter basename={baseUrl}>
			<App>
				<Helmet prioritizeSeoTags>
					{/* <title>Hello World</title> */}
					{/* <link rel="canonical" href="https://reactjs.org/tutorial/tutorial.html" /> */}
					{fonts}
				</Helmet>
			</App>
		</BrowserRouter>
	</HelmetProvider>
)

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://cra.link/PWA
serviceWorkerRegistration.unregister();

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals(console.log);
