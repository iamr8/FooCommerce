import React from 'react';
import { render, unmountComponentAtNode } from "react-dom";
import { act } from 'react-dom/test-utils';
import * as Popup from './../components/Popup';

var container: HTMLDivElement;
beforeEach(() => {
	// setup a DOM element as a render target
	container = document.createElement("div");
	document.body.appendChild(container);
});

afterEach(() => {
	// cleanup on exiting
	unmountComponentAtNode(container);
	container.remove();
	// container = null;
});

it("renders with or without a name", () => {
	act(() => {
		render(<Popup.Container title="Test" id="testModal">
			<Popup.Body></Popup.Body>
			<Popup.Footer></Popup.Footer>
		</Popup.Container>, container);
	});
	expect(container.firstElementChild).toBeNull();
});