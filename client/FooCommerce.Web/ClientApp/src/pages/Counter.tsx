import React, { Component } from 'react';
import { Helmet } from 'react-helmet-async';

interface IState {
	currentCount: number;
}

export class Counter extends Component<{}, IState> {
	static displayName = Counter.name;

	constructor(props: any) {
		super(props);
		this.state = { currentCount: 0 };
		this.incrementCounter = this.incrementCounter.bind(this);
	}

	incrementCounter() {
		this.setState({
			currentCount: this.state.currentCount + 1
		});
	}

	render() {
		return (
			<>
				<Helmet>
					<title>Counter</title>
				</Helmet>
				<h1>Counter</h1>

				<p>This is a simple example of a React component.</p>

				<p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

				<button type='button' className="btn btn-primary" onClick={this.incrementCounter}>Increment</button>
			</>
		);
	}
}
